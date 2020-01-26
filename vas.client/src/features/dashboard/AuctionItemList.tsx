import React, { useContext } from 'react'
import { RootStoreContext } from '../../app/stores/rootStore';
import { Segment, Item, Grid, Image } from 'semantic-ui-react';
import { observer } from "mobx-react-lite";

const AuctionItemList = () => {
    const rootStore = useContext(RootStoreContext);
    const { getAuctionItemsStore } = rootStore.auctionStore;
    
    return (
        <Segment clearing>
        <Item.Group divided>

          {getAuctionItemsStore.map(record => (
            <Item key={record.auctionId}>
              <Grid columns={2}>
                <Grid.Column>
                  <Image src={record.images[0]}/>
                </Grid.Column>
                <Grid.Column>
                  <Item.Content>
                    <Item.Header as="a">ID AUKCIJE: {record.auctionId}</Item.Header>
                    <Item.Content>MODEL: {record.model}</Item.Content>
                    <Item.Content>SNAGA: {record.horsePower}</Item.Content>
                    <Item.Content>KILOMETRAŽA: {record.mileage}</Item.Content>
                    <Item.Content>POČETNA CIJENA: {record.startPrice}</Item.Content>
                    <Item.Content>GODINA: {record.year}</Item.Content>
                    <Item.Content>ZADNJA CIJENA: {record.endPrice} -> KUPAC: {record.winner}</Item.Content>
                    <Item.Content>POČETAK AUKCIJE: {record?.auctionStart}</Item.Content>
                    <Item.Content>KRAJ AUKCIJE: {record?.auctionEnd}</Item.Content>
                  </Item.Content>
                </Grid.Column>
              </Grid>
            </Item>
          ))}

        </Item.Group>
      </Segment>
    )
}

export default observer(AuctionItemList);
