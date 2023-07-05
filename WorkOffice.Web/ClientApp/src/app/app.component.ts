import { Component } from '@angular/core';
import {
  MatSnackBar,
  MatSnackBarVerticalPosition,
  MatSnackBarHorizontalPosition,
} from '@angular/material/snack-bar';
import { Event, Router, NavigationStart, NavigationEnd } from '@angular/router';
import { DEFAULT_INTERRUPTSOURCES, Idle } from '@ng-idle/core';
import { AuthService } from './core/service/auth.service';
import { Keepalive } from '@ng-idle/keepalive';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  currentUrl!: string;
  numberOfSeconds = 300;
  lastPing?: Date;

  constructor(
    public _router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private router: Router,
    private idle: Idle,
    private keepalive: Keepalive
  ) {
    this._router.events.subscribe((routerEvent: Event) => {
      if (routerEvent instanceof NavigationStart) {
        this.currentUrl = routerEvent.url.substring(
          routerEvent.url.lastIndexOf('/') + 1
        );
      }
      if (routerEvent instanceof NavigationEnd) {
        /* empty */
      }
      window.scrollTo(0, 0);
    });
    this.idle.setIdle(this.numberOfSeconds);
    this.idle.setTimeout(10);
    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    this.idle.onIdleEnd.subscribe(() => {
     console.log("No longer idle")
    });

    this.idle.onTimeout.subscribe(() => {
     this.authService.logout().subscribe((res) => {
       this.router.navigate(['/account/login']);
   });
    });

    this.idle.onIdleStart.subscribe(() => {
       console.log('You\'ve gone idle!')
    });

    this.idle.onTimeoutWarning.subscribe((countdown) => {
     this.showNotification(
       'snackbar-danger',
       'You will time out in ' + countdown + ' seconds!',
       'top',
       'right'
     );
      });

    // sets the ping interval to 15 seconds
    this.keepalive.interval(15);

    this.keepalive.onPing.subscribe(() => this.lastPing = new Date());


      if (this.authService.currentUser) {
       this.idle.watch()
      } else {
       this.idle.stop();
      }
  }




  showNotification(
    colorName: string,
    text: string,
    placementFrom: MatSnackBarVerticalPosition,
    placementAlign: MatSnackBarHorizontalPosition
  ) {
    this.snackBar.open(text, '', {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }
}
