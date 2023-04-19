import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-pathway',
  templateUrl: './add-pathway.component.html',
  styleUrls: ['./add-pathway.component.css']
})
export class AddPathwayComponent implements OnInit {
display = false
  constructor() {
  //add code here
  }

  ngOnInit() {
    this.display = true
  }

}
