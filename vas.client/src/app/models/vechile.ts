export default interface IVehicle{
    auctionId: string;
    auctionStart: Date;
    auctionEnd: Date;
    model: string;
    horsePower: number;
    bidIncrement: number;
    endPrice: number;
    images: string[];
    year: number;
    mileage: number;
    startPrice: number;
    vehicleType: number;
    color: string;
    buyers: string[];
    winner: string;
}