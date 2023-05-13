import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

import { catchError } from 'rxjs/operators';
import { GeneralSettingsService } from '../service/general-settings.service';
import { GeneralSettingsModel } from '../models/general-settings.model';

@Injectable({
  providedIn: 'root'
})
export class CompanyStructureResolver implements Resolve<GeneralSettingsModel[]> {
  constructor(
    private generalSettingsService: GeneralSettingsService,
    private router: Router,
  ) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<any> {

    return this.generalSettingsService.getCompanyStructureList()
      .pipe(catchError((err) => this.router.navigateByUrl('/')));
  }
}
