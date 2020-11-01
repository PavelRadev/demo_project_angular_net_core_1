import { Component, OnInit } from '@angular/core';
import { SessionService } from '../../../core/services/session.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './app.header.component.html'
})
export class AppHeaderComponent implements OnInit {

  constructor(private sessionService: SessionService, private router: Router) {
  }

  ngOnInit(): void {
  }

  logout(): void {
    this.sessionService.PerformLogout();
    this.router.navigate(['auth/login']);
  }

}
