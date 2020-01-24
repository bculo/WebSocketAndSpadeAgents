from datetime import datetime, timedelta

class Vechile:
    
    def __init__(self, auctionId, model, horsePower, bidIncrement, startPrice, year, mileage, color, vechileType, winner):
        self.auctionId = auctionId
        self.model = model
        self.horsePower = horsePower
        self.bidIncrement = bidIncrement
        self.endPrice = startPrice
        self.year = year
        self.auctionStart = None,
        self.auctionEnd = None
        self.mileage = mileage
        self.startPrice = startPrice
        self.type = vechileType
        self.color = color
        self.buyers = []
        self.type = vechileType
        self.winner = winner

    #print instance info
    def printInfo(self):
        print(f"Brand: '{self.model}', Type '{self.type}'")
    
    #set auction start
    def setAuctionStart(self, offset):
        #offset section (working with seconds here) variables
        initalOffset = 60
        offsetForAuctionStart = initalOffset + offset
        itemDurationTime = 30
        offsetForEndTime = offsetForAuctionStart + itemDurationTime

        #offset calc
        temp = datetime.now() 
        self.auctionStart = temp + timedelta(seconds=offsetForAuctionStart)    
        self.auctionEnd = temp + timedelta(seconds=offsetForEndTime)

        print(f"Start: {self.auctionStart} --- End: {self.auctionEnd}")