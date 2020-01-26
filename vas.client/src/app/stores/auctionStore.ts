import { action, observable, runInAction, computed } from "mobx";
import { RootStore } from "./rootStore";
import IVehicle from "../models/vechile";
import IChangePriceRequestModel from "../models/price";
import agent from "../api/agent";

export default class UserStore {
  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable auctionRegistry = new Map();

  @computed get getAuctionItemsStore(): IVehicle[] {
    return Array.from(this.auctionRegistry.values());
  }

  @action getAuctionItemsApi = async () => {
    try {
      const response = await agent.Auction.getAuctionItems();
      runInAction(() => {
        response.forEach(item => {
          this.auctionRegistry.set(item.auctionId, item);
        });
      });
    } catch (error) {
      console.log(error);
    }
  };

  @action startBuyer = async(email: string) => {
    try{
        await agent.Auction.startBuyer({email: email});
    }catch(error){
        console.log(error);
    }
  }

  @action updateAuctionItem = (info: string) => {
    console.log("UPDATE PRICE")
    var instance: any = JSON.parse(info);
    var auctionItemPriceUpdate: IChangePriceRequestModel = instance.Result;
    var itemFromList: IVehicle = this.auctionRegistry.get(auctionItemPriceUpdate.auctionId);
    itemFromList.winner = auctionItemPriceUpdate.winner;
    itemFromList.endPrice = auctionItemPriceUpdate.endPrice;
    this.auctionRegistry.set(itemFromList.auctionId, itemFromList);
  }
}
