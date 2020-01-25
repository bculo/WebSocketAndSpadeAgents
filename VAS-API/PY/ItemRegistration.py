class ItemRegistration:
    def __init__(self, agentId):
        self.agentId = agentId
        self.auctionItems = []
    
    def addAuctionItem(self, auctionId):
        self.auctionItems.append(auctionId)