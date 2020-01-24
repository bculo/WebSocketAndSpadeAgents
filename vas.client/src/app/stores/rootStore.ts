import { createContext } from 'react';
import { configure } from 'mobx';
import AuctionStore from "./auctionStore";

configure({enforceActions: 'always'});

export class RootStore{
    auctionStore: AuctionStore;

    constructor(){
        this.auctionStore = new AuctionStore(this);
    }
}

export const RootStoreContext = createContext(new RootStore());