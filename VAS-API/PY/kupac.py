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
from ast import literal_eval
from aioxmpp import JID

class Buyer(Agent):

    class Registration(OneShotBehaviour):

        async def run(self):

            #create spade message instance
            msg = Message(
                to="bogokdu@jabber.eu.org",
                body = self.agent.id,
                metadata = {
                    "ontology": "aukcija",
                    "language": "hrvatski",
                    "intent": "registerbuyer"
                    }
            )
            
            #send message to receiver
            await self.send(msg)

            print("poruka poslana !")


    #program start point
    async def setup(self):
        #set id
        self.id = self.getId()
        print(self.id)

        #add registration behaviour
        registrationBehaviour = self.Registration()
        self.add_behaviour(registrationBehaviour)

    def getId(self):
        return f"{self.name}@{self.jid.domain}"

    #write message -> prints agent name and given message
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

    #create agent instance and start it
    kupac = Buyer(inputArgs.user, inputArgs.pw)
    kupac.start()

#start program with: py kupac.py -user darkogajic@jix.im -pw darKolozinka55