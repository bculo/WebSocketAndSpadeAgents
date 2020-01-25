import { action, observable, runInAction, computed } from "mobx";
import { RootStore } from "./rootStore";
import IVehicle from "../models/vechile";
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
}
