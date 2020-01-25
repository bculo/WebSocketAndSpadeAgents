#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import spade
from spade.agent import Agent
from spade.behaviour import OneShotBehaviour, CyclicBehaviour
from spade.message import Message
from spade.template import Template

import sys
from time import sleep
import argparse
import jsonpickle
import requests
import json
from ast import literal_eval
from aioxmpp import JID
from datetime import datetime, timedelta
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

from Vehicle import Vechile
from ItemRegistration import ItemRegistration

class WantedItem:
    def __init__(self, model, year, horsePower, milage, budget):
        self.model = model
        self.year = year
        self.horsePower = horsePower
        self.milage = milage
        self.budget = budget

    def printInfo(self):
        print(f"{self.model} | {self.year} | {self.horsePower} | {self.milage} | {self.budget}")

class Buyer(Agent):

    #komunikacija za aukciju
    class Communication(CyclicBehaviour):

        async def run(self):
            msg = await self.receive(timeout=500)

            if msg:
                intent = msg.get_metadata("intent")
                print(f"PRIMIO PORUKU SA NAMJENOM: {intent}")
            else:
                print("Nema poruke")


    #registracija korisnika
    class Registration(OneShotBehaviour):

        async def run(self):
            #registriraj se 
            msg = Message(
                to= self.agent.auctioneer,
                body = self.agent.id,
                metadata = {
                    "ontology": "aukcija",
                    "language": "hrvatski",
                    "intent": "registerbuyer"
                    }
            )
            await self.send(msg)
            print("Poruka za registraciju poslana !")

            #dohvati sve proizvode od organizatora
            response = await self.receive(timeout=100)
            if response:
                print("Odgovor na registraciju stigao")
                intent = response.get_metadata("intent")
                if intent == "auctionitems":
                    temp = jsonpickle.decode(response.body)
                    tempArray = []
                    for item in temp:
                        item.__class__ = Vechile
                        tempArray.append(item)
                    self.agent.possibleAuctions = tempArray

                    numOfAuctionsItems = len(self.agent.possibleAuctions)
                    print(f"Total auction items to bid before filter: {numOfAuctionsItems}")

                    self.agent.filterItems()
            else:
                print("Poruka od organizatora nije stigla")
                return
            
            #prijavi se na zeljene aukcije
            reg = ItemRegistration(self.agent.id)
            for auctionItem in self.agent.finalAuctions:
                reg.addAuctionItem(auctionItem.auctionId)
            content = jsonpickle.encode(reg)
            msg = Message(
                to= self.agent.auctioneer,
                body = content,
                metadata = {
                    "ontology": "aukcija",
                    "language": "hrvatski",
                    "intent": "auctionitemsreg"
                    }
            )
            await self.send(msg)

    #pocetak rada
    async def setup(self):
        self.id = self.getId()

        self.auctioneer = self.getAuctioneerInfo()
        if self.auctioneer == None:
            return

        self.possibleAuctions = []
        self.finalAuctions = []
        self.wantedItems = self.getWantedItems()

        #dodavanje ponasanja
        registrationBehaviour = self.Registration()
        self.add_behaviour(registrationBehaviour)
        communictionTemplate = Template(metadata = {"ontology": "bid"})
        communictionBehaviour = self.Communication()
        self.add_behaviour(communictionBehaviour, communictionTemplate)

    #dohvati mail osobe koja vodi aukciju sa apija
    def getAuctioneerInfo(self):
        apiUrl = "https://localhost:44388/api/auction/getauctioneerinfo"
        headers = {'Content-Type': 'application/json'}
        response = requests.get(url=apiUrl, headers=headers, timeout=10, verify=False)

        if response.status_code == 200: #success
            return response.content.decode()
        else: #error
            return None

    #filtriraj sve aukcije samo onima koje zanimaju kupca
    def filterItems(self):
        for a in self.possibleAuctions:
            for w in self.wantedItems:
                if w.model not in a.model: #dobar model ?
                    continue

                if w.year < a.year: #dobra godina
                    continue

                if w.horsePower < a.horsePower: #dobra snaga
                    continue

                if w.milage > a.mileage: #dobra kilometraza
                    continue

                if w.budget > a.startPrice: #dobar budget
                    continue

                self.finalAuctions.append(a)
        print(f"{self.id} sudjelujem na {len(self.finalAuctions)} aukcija")

    #dohvati aukcije koje zanimaju kupca
    def getWantedItems(self):
        apiUrl = f"https://localhost:44388/api/auction/getuserlist/{self.getId()}"
        headers = {'Content-Type': 'application/json'}
        response = requests.get(url=apiUrl, headers=headers, timeout=10, verify=False)
        
        if response.status_code == 200: #success
            tempArray = []
            for item in response.json():
                model = item["model"]
                horsePower = item["horsePower"]
                year = item["year"]
                mileage = item["milage"]
                budget = item["budget"]
                auctionItem = WantedItem(model, year, horsePower, mileage, budget)
                tempArray.append(auctionItem)
            return tempArray
        else: #api call failed
            print("API problem!!!")
            return []

    #dohvati id agenta
    def getId(self):
        return f"{self.name}@{self.jid.domain}"

    #prikazi informacije
    def say(self, line):
        print(f"{self.name}: {line}")
    
    #zaustavi agenta
    def stopAgent(self):
        print(f"Zaustavljam agenta {self.id}")
        self.stop()

if __name__ == '__main__':
    #create parser for reading input arguments
    parser = argparse.ArgumentParser()

    #define possible input arguments
    parser.add_argument("-user", type=str, help="Identifikator kupca")
    parser.add_argument("-pw", type=str, help="Password kupca")

    #parse input arguments
    inputArgs = parser.parse_args()

    #create agent instance and start it
    kupac = Buyer(inputArgs.user, inputArgs.pw)
    kupac.start()