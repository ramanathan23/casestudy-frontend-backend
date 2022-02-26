import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SearchService } from 'src/Services/search.service';
import { ProductBidDetailsModel } from '../Models/ProductBidDetailsModel';

@Component({
  selector: 'app-prodcut',
  templateUrl: './prodcut.component.html',
  styleUrls: ['./prodcut.component.css']
})
export class ProdcutComponent implements OnInit, OnDestroy {
  productBidDetailsSubscription: Subscription | null = null;
  productDetails: ProductBidDetailsModel | null = null;

  constructor(private searchService: SearchService) { }

  ngOnInit(): void {
    this.productBidDetailsSubscription = this.searchService.productBidDetailsObservable.subscribe(
      (result) => this.productDetails = result
    );
  }

  ngOnDestroy(): void {
    this.productBidDetailsSubscription?.unsubscribe();
  }

}
