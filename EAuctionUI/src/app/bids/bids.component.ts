import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SearchService } from 'src/Services/search.service';
import { BidDetailsModel } from '../Models/BidDetailsModel';

@Component({
  selector: 'app-bids',
  templateUrl: './bids.component.html',
  styleUrls: ['./bids.component.css']
})
export class BidsComponent implements OnInit {
  productBidDetailsSubscription: Subscription | null = null;
  bidDetails: BidDetailsModel[] | null = [];

  constructor(private searchService: SearchService) { }

  ngOnInit(): void {
    this.productBidDetailsSubscription = this.searchService.productBidDetailsObservable.subscribe(
      (result) => this.bidDetails = result.bids
    );
  }

  ngOnDestroy(): void {
    this.productBidDetailsSubscription?.unsubscribe();
  }

}
