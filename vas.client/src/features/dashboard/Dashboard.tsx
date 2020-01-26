import React, { Fragment, useContext, useEffect } from 'react'
import { Header, Button, Container } from 'semantic-ui-react'
import { RootStoreContext } from '../../app/stores/rootStore'
import AuctionItemList from "./AuctionItemList";
import { observer } from "mobx-react-lite";
import * as signalR from "@microsoft/signalr";

const Dashboard = () => {
    const rootStore = useContext(RootStoreContext);
    const { getAuctionItemsApi, startBuyer } = rootStore.auctionStore;

    useEffect(() => {
        getAuctionItemsApi()
        connectToHub()
    }, [getAuctionItemsApi]);

    function connectToHub(){
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44388/auction")
            .build();

        connection.start()
            .then(() => console.log("CONNECTED TO HUB"))
            .catch(error => console.log(error));

        connection.on("AddBuyer", (message: string) => {
            console.log("ADDBUYER: " + message)
        });

        connection.on("PriceChanged", (message: string) => {
            getAuctionItemsApi();
        });

        connection.on("AuctionFinished", (message: string) => {
            getAuctionItemsApi();
        });

        connection.on("UpdateTimes", (message: string) => {
            getAuctionItemsApi();
        });
    }

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
