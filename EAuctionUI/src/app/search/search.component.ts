import { Component, OnInit } from '@angular/core';
import { SearchService } from 'src/Services/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  constructor(private searchService: SearchService) { }

  ngOnInit(): void {
  }

  doSearch(value:string){
      this.searchService.SearchAuctionProduct(value).subscribe(
        (result) => {
          this.searchService.productBidDetailsSubject.next(result);
        }
      );
  }
}
