#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import spade
from spade.agent import Agent
from spade.behaviour import OneShotBehaviour, CyclicBehaviour, PeriodicBehaviour
from spade.message import Message
from spade.template import Template

# importing the requests library 
import requests
import json
import jsonpickle
from datetime import datetime, timedelta
import traceback

import sys
from time import sleep
import argparse
from ast import literal_eval
from datetime import datetime, timedelta

import urllib3
from enum import Enum

from Vehicle import Vechile
from ItemRegistration import ItemRegistration
from Bid import Bid
from NewBid import NewBid
from Winner import Winner

urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

#time request update
class TimeUpdateRequest:
    def __init__(self, auctionId, auctionStart, auctionEnd):
        self.auctionId = auctionId
        self.auctionStart = auctionStart
        self.auctionEnd = auctionEnd

#add new buyer request model
class AddBuyerRequest:
    def __init__(self, auctionId, buyer):
        self.auctionId = auctionId
        self.buyer = buyer

#change price model
class ChangePriceRequest:
    def __init__(self, auctionId, endPrice):
        self.auctionId = auctionId
        self.endPrice = endPrice

#end auction for this item model
class EndAuctionRequestModel:
    def __init__(self, auctionId, endPrice, winner):
        self.auctionId = auctionId
        self.endPrice = endPrice
        self.winner = winner

#auction logic
class Organizer(Agent):

    #vodenje aukcije
    class Auction(PeriodicBehaviour):

        async def run(self):
            for auctionElement in self.agent.auctions:

                if auctionElement.auctionStart < datetime.now():
                    if auctionElement.auctionEnd > datetime.now():

                        if len(auctionElement.buyers) > 0:
                            for buyer in auctionElement.buyers:
                                bid = Bid(auctionElement.auctionId, auctionElement.bidIncrement, auctionElement.winner, auctionElement.endPrice)
                                bidJson = jsonpickle.encode(bid)
                                msgNotify = Message(
                                    to= buyer,
                                    body = bidJson,
                                    metadata = {
                                        "ontology": "aukcija",
                                        "language": "hrvatski",
                                        "intent": "bidajakooces"
                                    }
                                )
                                await self.send(msgNotify)
                        else:
                            print(f"AUKCIJA {auctionElement.auctionId} NEMA SUDIONIKA")
                    
                        if auctionElement.winner != None:
                            print(f"AUKCIJU VODI AGENT {auctionElement.winner} -> {auctionElement.endPrice}")

                    else:
                        if auctionElement.active == True:                    
                            if auctionElement.winner != None:
                                print(f"AUKCIJU VODI AGENT {auctionElement.winner} -> {auctionElement.endPrice}")
                                
                                update = EndAuctionRequestModel(auctionElement.auctionId, auctionElement.endPrice, auctionElement.winner)
                                updateJson = jsonpickle.encode(update)
                                apiUrl = "https://localhost:44388/api/auction/finish"
                                headers = {'Content-Type': 'application/json'}
                                requests.put(url=apiUrl, data=updateJson, headers=headers, timeout=10, verify=False)
                                
                            if len(auctionElement.buyers) > 0:
                                for buyer in auctionElement.buyers:
                                    win = Winner(auctionElement.auctionId, auctionElement.winner, auctionElement.endPrice)
                                    winJson = jsonpickle.encode(win)
                                    msgNotify = Message(
                                        to= buyer,
                                        body = winJson,
                                        metadata = {
                                            "ontology": "aukcija",
                                            "language": "hrvatski",
                                            "intent": "aukcijagotova"
                                        }
                                    )
                                    await self.send(msgNotify)
                            
                            else:
                                print(f"AUKCIJA {auctionElement.auctionId} NEMA SUDIONIKA")
                            
                            print("ZAVRSAVAM AUKCIJU ZA OVAJ PREDMET")
                            auctionElement.active = False

    #registracija korisnika
    class Communication(CyclicBehaviour):
        async def run(self):
            try:
                msg = await self.receive(timeout=100)
                if msg:
                    intent = msg.get_metadata("intent")

                    #register buyer
                    if intent == "registerbuyer":
                        #check if user is in registeredUser array
                        if msg.body not in self.agent.registeredUsers:
                            self.agent.registeredUsers.append(msg.body)
                            print(f"User {msg.body} registered")
                        else:
                            print(f"User {msg.body} already added")

                        #filter list with available auction items
                        tempAuctions = []
                        for auctionItem in self.agent.auctions:
                            if auctionItem.auctionStart > datetime.now():
                                tempAuctions.append(auctionItem)

                        #create spade message instance for registration
                        content = jsonpickle.encode(self.agent.auctions)
                        msg = Message(
                            to= msg.body,
                            body = content,
                            metadata = {
                                "ontology": "aukcija",
                                "language": "hrvatski",
                                "intent": "auctionitems"
                                }
                            )
                        await self.send(msg)
                        print("ORGANIZATOR: Poruka aukcija poslana")
                    
                    #register buyer auctions
                    if intent == "auctionitemsreg":
                        temp = jsonpickle.decode(msg.body)
                        temp.__class__ = ItemRegistration
                        for auction in self.agent.auctions:
                            for registeredAuction in temp.auctionItems:
                                if auction.auctionId == registeredAuction:
                                    print(f"Korisnik {temp.agentId} prijavljen na aukciju {auction.auctionId}")
                                    auction.buyers.append(temp.agentId)

                                    update = AddBuyerRequest(auction.auctionId, temp.agentId)
                                    updateJson = jsonpickle.encode(update)
                                    apiUrl = "https://localhost:44388/api/auction/buyer"
                                    headers = {'Content-Type': 'application/json'}
                                    requests.put(url=apiUrl, data=updateJson, headers=headers, timeout=10, verify=False)
                    
                    #biding
                    if intent == "bid":
                        bider = jsonpickle.decode(msg.body)
                        bider.__class__ = NewBid
                        for auction in self.agent.auctions:
                            if auction.auctionId == bider.auctionId:
                                if auction.endPrice < bider.agentPrice:
                                    auction.endPrice = bider.agentPrice
                                    auction.winner = bider.agentId

                                    update = EndAuctionRequestModel(auction.auctionId, auction.endPrice, auction.winner)
                                    updateJson = jsonpickle.encode(update)
                                    apiUrl = "https://localhost:44388/api/auction/newprice"
                                    headers = {'Content-Type': 'application/json'}
                                    requests.put(url=apiUrl, data=updateJson, headers=headers, timeout=10, verify=False)
                                else:
                                    bid = Bid(auction.auctionId, auction.bidIncrement, auction.winner, auction.endPrice)
                                    bidJson = jsonpickle.encode(bid)
                                    msgNotify = Message(
                                        to= bider.agentId,
                                        body = bidJson,
                                        metadata = {
                                            "ontology": "aukcija",
                                            "language": "hrvatski",
                                            "intent": "bidajakooces"
                                        }
                                    )
                                    await self.send(msgNotify)                                    

            except Exception:
                traceback.print_exc()

    #pocetna tocka
    async def setup(self):
        self.auctions = self.getAuctionItems()
        self.updateAuctionTimes(self.auctions)
        self.registeredUsers = []

        komunikacijaPonasanje = self.Communication()
        self.add_behaviour(komunikacijaPonasanje)
        auctionBehaviour = self.Auction(period=5)
        self.add_behaviour(auctionBehaviour)

    #update start and end time of auction item
    def updateAuctionTimes(self, auctions):
        #offset variables
        offset = 60
        totalOffset = 0
        for item in auctions:
            item.__class__ = Vechile
            item.printInfo()
            item.setAuctionStart(totalOffset)

            request = TimeUpdateRequest(item.auctionId, item.auctionStart.isoformat(), item.auctionEnd.isoformat())
            tempData = json.dumps(request.__dict__)
            apiUrl = "https://localhost:44388/api/auction/setdates"
            headers = {'Content-Type': 'application/json'}
            response = requests.post(url=apiUrl, data=tempData, headers=headers, timeout=10, verify=False)

            if response.status_code == 200: #success
                totalOffset = totalOffset + offset
                print("Auction start and end time updated")
            else: #problem with api
                print("Problem with API")

        print("UPDATE CLIENT API CALL")
        apiUrl = "https://localhost:44388/api/auction/updateclient"
        headers = {'Content-Type': 'application/json'}
        response = requests.get(url=apiUrl, headers=headers, timeout=10, verify=False)

    #get items from API
    def getAuctionItems(self):
        #api call
        apiUrl = "https://localhost:44388/api/auction/get"
        headers = {'Content-Type': 'application/json'}
        response = requests.get(url=apiUrl, headers=headers, timeout=10, verify=False)
        
        if response.status_code == 200: #success
            tempArray = []
            for item in response.json():
                auctionId = item["auctionId"]
                model = item["model"]
                horsePower = item["horsePower"]
                bidIncrement = item["bidIncrement"]
                startPrice = item["startPrice"]
                year = item["year"]
                mileage = item["mileage"]
                vehicleType = item["type"]
                color = item["color"]
                winner = item["winner"]
                auctionItem = Vechile(auctionId, model, horsePower, bidIncrement, startPrice, year, mileage, color, vehicleType, winner)
                tempArray.append(auctionItem)
            return tempArray
        else: #api call failed
            print("API problem!!!")
            return []

    #print info
    def say(self, line):
        print(f"{self.name}: {line}")

#main
if __name__ == '__main__':
    #create parser for reading input arguments
    parser = argparse.ArgumentParser()

    #define possible input arguments
    parser.add_argument("-user", type=str, help="Identifikator kupca")
    parser.add_argument("-pw", type=str, help="Password kupca")

    #parse input arguments
    inputArgs = parser.parse_args()

    #create agent instance
    organizator = Organizer(inputArgs.user, inputArgs.pw)
    organizator.start()