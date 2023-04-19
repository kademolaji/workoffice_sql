import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-patient-document',
  templateUrl: './add-patient-document.component.html',
  styleUrls: ['./add-patient-document.component.css']
})
export class AddPatientDocumentDialogComponent implements OnInit {
display = true;
  constructor() {
    //code here
  }

  ngOnInit() {
    this.display = false;
  }

}
