import React, { useContext } from 'react'
import { RootStoreContext } from '../../app/stores/rootStore';
import { Segment, Item } from 'semantic-ui-react';
import { observer } from "mobx-react-lite";

const AuctionItemList = () => {
    const rootStore = useContext(RootStoreContext);
    const { getAuctionItemsStore } = rootStore.auctionStore;
    
    return (
        <Segment clearing>
        <Item.Group divided>

          {getAuctionItemsStore.map(record => (
            <Item key={record.auctionId}>
              <Item.Content>
                <Item.Header as="a">{record.auctionId}</Item.Header>
                <Item.Content>{record.horsePower}</Item.Content>
              </Item.Content>
            </Item>
          ))}

        </Item.Group>
      </Segment>
    )
}

export default observer(AuctionItemList);
