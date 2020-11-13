import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
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
  constructor(private basketService: BasketService, private accountService: AccountService) {}

  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }
  // tslint:disable-next-line: typedef
  loadBasket() {
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
  // tslint:disable-next-line: typedef
  loadCurrentUser() {
      // check if a token is stored in client's local storage, if so, get the corresponding
    // user back, so that the logined in user state can be persisted
    const token = localStorage.getItem('token');

    // 不管有没有token，都 call accountService的loadCurrentUser method
    this.accountService.loadCurrentUser(token).subscribe(() => {
        console.log('loaded user');
      }, error => {
        console.log(error);
      });
  }
}
