import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-validate-now',
  templateUrl: './validate-now.component.html',
  styleUrls: ['./validate-now.component.css']
})
export class ValidateNowComponent implements OnInit {
display = false
  constructor() {
    //add code here
   }

  ngOnInit() {
    this.display = true
  }

}
