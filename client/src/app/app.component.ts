import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { IBasket } from './shared/models/basket';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
// implement OnInit interface to be able to use ngOnInit()
export class AppComponent implements OnInit {
  title = 'Skinet';
  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    // check if a basket id is stored in client's local storage
    const basketId = localStorage.getItem('basket_id');
    if (basketId){
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initialised basket');
      }, error => {
        console.log(error);
      });
    }
  }
}
