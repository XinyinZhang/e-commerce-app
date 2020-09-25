import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
// implement OnInit interface to be able to use ngOnInit()
export class AppComponent implements OnInit {
  title = 'Skinet';
  constructor() {}

  ngOnInit(): void {
  }
}
