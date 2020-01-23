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

import sys
from time import sleep
import argparse
from ast import literal_eval
from datetime import datetime, timedelta

from enum import Enum

#enum for vechile type 
class Type(Enum):
    CAR = 1
    BIKE = 2

#auction object
class Vechile:
    
    def __init__(self, model, brand, startPrice, vechileType):
        self.model = model
        self.brand = brand
        self.startPrice = startPrice
        self.buyers = []
        self.lastBuyer = None
        self.active = True
        self.type = vechileType

    def printInfo(self):
        print(f"Brand: '{self.brand}', Model: '{self.model}', Type '{self.type}'")

#auction logic
class Organizer(Agent):

    #registracija korisnika
    class Komunikacija(CyclicBehaviour):
        async def run(self):
            #wait message
            try:
                msg = await self.receive(timeout=100)
                #check successfully received
                if msg:

                    #check intent
                    intent = msg.get_metadata("intent")
                    if intent == "registerbuyer":
                        print(msg.body)

                else:
                    print("Cekam nove sudionike...")
            
            except:
                print("Error ocured")



    async def setup(self):
        self.auctions = self.getAuctionItems()
    
        self.buyers = []

        komunikacijaPonasanje = self.Komunikacija()
        self.add_behaviour(komunikacijaPonasanje)


    #get items from API
    def getAuctionItems(self):

        #api call
        apiUrl = "https://localhost:44388/api/auction/get"
        headers = {'Content-Type': 'application/json'}
        response = requests.get(url=apiUrl, headers=headers, timeout=10, verify=False)
        
        if response.status_code == 200: #success
            tempArray = []
            for item in response.json():
                model = item["model"]
                brand = item["brand"]
                startPrice = item["startPrice"]
                vechileType = item["type"]
                vechileObject = Vechile(model, brand, startPrice, vechileType)
                vechileObject.printInfo()
                tempArray.append(vechileObject)
            return tempArray
                
        else: #api call failed
            return []

    def say(self, line):
        print(f"{self.name}: {line}")

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