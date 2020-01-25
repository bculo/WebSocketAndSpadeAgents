import React, { Fragment, useContext, useEffect } from 'react'
import { Header, Button, Segment, Container, Item } from 'semantic-ui-react'
import { RootStoreContext } from '../../app/stores/rootStore'
import AuctionItemList from "./AuctionItemList";
import { observer } from "mobx-react-lite";

const Dashboard = () => {
    const rootStore = useContext(RootStoreContext);
    const { getAuctionItemsApi, startBuyer } = rootStore.auctionStore;

    useEffect(() => {
        getAuctionItemsApi()
    }, [getAuctionItemsApi]);

    return (
        <Fragment>
            <Header as="h1" icon textAlign="center" style={{marginTop:"50px"}}>
                <Header.Content>VAS</Header.Content>
            </Header>

            <Container textAlign='center'>
                <Button content="START FIRST AGENT" onClick={() => startBuyer("dibo123@jabber.eu.org")} color="blue" style={{width:"300px"}}/>
                <Button content="START SECOND AGENT" onClick={() => startBuyer("igorm@jabber.eu.org")} color="blue" style={{width:"300px"}}/>
            </Container>

            <Container style={{marginTop: "50px"}}>
                <AuctionItemList/>
            </Container>
        </Fragment>
    )
}

export default observer(Dashboard);
