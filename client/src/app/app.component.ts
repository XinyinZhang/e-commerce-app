import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IProduct } from './models/product';
import { IPagnination } from './models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
// implement OnInit interface to be able to use ngOnInit()
export class AppComponent implements OnInit {
  title = 'Skinet';
  products: IProduct[];
  constructor(private http: HttpClient) {}

  ngOnInit(): void { // called only once, to initialize the component,
                    // set and display its input properties
      // fetch data from database
      this.http.get('https://localhost:5001/api/products?pageSize=50').subscribe
      ((response: IPagnination) => {
        this.products = response.data;

      }, error => {
        console.log(error);
      });
  }
}
