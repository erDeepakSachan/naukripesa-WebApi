import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { AuthService } from './../shared/services/auth.service';
import { NeoContextService } from './../shared/services/neo-context.service';


@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, AfterViewInit {

  constructor(private authSvc: AuthService, public ctxSvc: NeoContextService,private titleService: Title) { }

  ngAfterViewInit(): void {
    this.titleService.setTitle(this.ctxSvc.context.AppSettings?.AppName!);
  }

  ngOnInit() {
  }

  email = '';
  password = '';

  login() {
    this.authSvc.login(this.email, this.password);
  }
}
