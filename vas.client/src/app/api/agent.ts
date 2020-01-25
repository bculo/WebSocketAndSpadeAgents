import axios, { AxiosResponse } from "axios";
import IVehicle from "../models/vechile";
import IUser from "../models/user";

//API BASE URL
axios.defaults.baseURL = "https://localhost:44388/api";

//RESPONSE
const responseBody = (response: AxiosResponse) => response.data;

//BASIC REQUESTS
const requesttype = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody)
};

const Auction = {
  getAuctionItems: (): Promise<IVehicle[]> => requesttype.get(`/auction/get`),
  startBuyer: (buyer: IUser) => requesttype.post(`/auction/startbuyer`, buyer),
}

export default {
  Auction,
};