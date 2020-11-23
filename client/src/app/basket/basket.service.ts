import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { IBasket, IBasketItem, Basket, IBasketTotals } from '../shared/models/basket';
import { map } from 'rxjs/operators';
import { IProduct } from '../shared/models/product';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>(null);
  basket$ = this.basketSource.asObservable(); // get an observable from behaviour subject
  // using the asObservable() method

  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basketTotal$ = this.basketTotalSource.asObservable();
  shipping = 0;
  constructor(private http: HttpClient) { }

  setShippingPrice(deliveryMethod: IDeliveryMethod): void
  {
    this.shipping = deliveryMethod.price;
    this.calculateTotals();
  }

  // tslint:disable-next-line: typedef
  getBasket(id: string){
    return this.http.get(this.baseUrl + 'basket?id=' + id)
    .pipe(
      map((basket: IBasket) => { // for each basket we get from the server, we call behaviourSubject
        // basketSource's next method to emit this basket to its subscribed observers
        this.basketSource.next(basket);
        this.calculateTotals();
    }));
  }
  // tslint:disable-next-line: typedef
  setBasket(basket: IBasket) { // store the basket into database
    return this.http.post(this.baseUrl + 'basket', basket).subscribe((response: IBasket) => {
      this.basketSource.next(response);
      this.calculateTotals();
      // http response will return the basket we posted as an observerable,
     // we first subscribe to this observable so we can get the response back; we then call basketSource to
     // emit this response to its subscribed observer
    }, error => {
      console.log(error);
    });
  }

  // public method: we want to instantly get the current value of the basket without actually subscribing to anything
  // tslint:disable-next-line: typedef
  getCurrentBasketValue() {
    return this.basketSource.value; // two ways to get current value from behaviourSubject:
    // 1. subscribe to it, 2. access the .value property without subscribtion
  }

  // the method to actually add an product to our basket
  // tslint:disable-next-line: typedef
  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket); // store the basket into database
  }
  incrementItemQuantity(item: IBasketItem): void{
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);
    basket.items[itemIndex].quantity++;
    this.setBasket(basket);

  }
  decrementItemQuantity(item: IBasketItem): void{
    const basket = this.getCurrentBasketValue();
    const itemIndex = basket.items.findIndex(x => x.id === item.id);
    if (basket.items[itemIndex].quantity > 1){
      basket.items[itemIndex].quantity--;
      this.setBasket(basket);
    } else {
      this.removeItemFromBasket(item);
    }

  }
  removeItemFromBasket(item: IBasketItem): void {
    const basket = this.getCurrentBasketValue();
    // a boolean return whether this item exist in the basket
    if (basket.items.some(x => x.id === item.id)){
      basket.items = basket.items.filter(i => i.id !== item.id); // return the array without the item
      if (basket.items.length > 0) {
        this.setBasket(basket);
      } else {
        // if no item anymore, remove basket
        this.deleteBasket(basket);
      }
    }


  }
  // tslint:disable-next-line: typedef
  deleteBasket(basket: IBasket){
    // generate an http request to delete basket from database
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next(null);
      this.basketTotalSource.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  // delete basket locally (will be called once an order is successfully created)
  // idea: once an order is successfully created on server side, server will delete the
  // basket from their database, we also want to delete this basket from local storage
  deleteLocalBasket(id: string): void
  {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private addOrUpdateItem(items: IBasketItem[], ItemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    // check if the item is already in items[], if no then push item, otherwise increase
    // the quantity of the item
    const index = items.findIndex(i => i.id === ItemToAdd.id);
    if (index === -1){
      ItemToAdd.quantity = quantity;
      items.push(ItemToAdd);
    }
    else {
      items[index].quantity += quantity;
    }
    return items;
  }
  private createBasket(): IBasket {
    // we want to store the basket id in the local storage on the client's browser,
    // so that when the user comes in to our application/restart our application,
    // we got the id of the basket and we can retrieve it from our API when the application starts
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;

  }
  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
   return {
     id: item.id,
     productName: item.name,
     price: item.price,
     pictureUrl: item.pictureUrl,
     quantity,
     brand: item.productBrand,
     type: item.productType
   };
  }

  // tslint:disable-next-line: typedef
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = this.shipping;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + shipping;
    this.basketTotalSource.next({shipping, total, subtotal});
  }

}
