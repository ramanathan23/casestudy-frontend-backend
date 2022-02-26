import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { ProductBidDetailsModel } from 'src/app/Models/ProductBidDetailsModel';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  productBidDetailsSubject: Subject<ProductBidDetailsModel>;
  productBidDetailsObservable: Observable<ProductBidDetailsModel>;

  constructor(private httpClient: HttpClient) {
     this.productBidDetailsSubject = new Subject();
     this.productBidDetailsObservable = this.productBidDetailsSubject.asObservable();
   }

  SearchAuctionProduct(productId: string): Observable<ProductBidDetailsModel> {
      return this.httpClient.get<ProductBidDetailsModel>(
        environment.endpoint + `/e-auction/api/v1/seller/show-bids/${productId}`,
       {'headers': new HttpHeaders().set('authorize-key','eauctionsecret321') });
  }

}
