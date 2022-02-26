import { BidDetailsModel } from "./BidDetailsModel";

export class ProductBidDetailsModel
{
    constructor(){}

    productName: string = ""
    shortDescription: string = ""
    category: string = ""
    startingPrice: number = 0
    bidEndDate: Date = new Date();
    bids: BidDetailsModel[] = []
}
