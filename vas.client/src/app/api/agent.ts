import axios, { AxiosResponse } from "axios";

//API BASE URL
axios.defaults.baseURL = "https://localhost:5001/api/v1";

//RESPONSE
const responseBody = (response: AxiosResponse) => response.data;

//BASIC REQUESTS
const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody)
};

const Auction = {
    
}

export default {
  Auction,
};