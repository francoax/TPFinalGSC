import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/Authentication/auth.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  constructor(private authenticationService : AuthenticationService) { }

  ngOnInit(): void {
  }

  logout() {
    this.authenticationService.logOut();
  }

}
