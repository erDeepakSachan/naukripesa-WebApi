import { Component, OnInit, AfterViewInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NeoContextService } from '../../shared/services/neo-context.service';
import { AppSettings } from '../../shared/objects/app-settings';
import { AuthService } from '../../shared/services/auth.service';
import { LeftMenuComponent } from './../../left-menu/left-menu.component';


@Component({
  selector: 'app-main-layout',
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.css'],
  imports: [RouterOutlet, CommonModule, LeftMenuComponent],
})
export class MainLayoutComponent implements OnInit {
  title = 'app';
  appSettings: AppSettings;
  currentYear: number = new Date().getFullYear();
  displayName: string = '';

  constructor(private titleService: Title
    , private neoContextSrv: NeoContextService
    , private authService: AuthService) {
    this.appSettings = neoContextSrv.context.AppSettings!;
    this.titleService.setTitle(this.appSettings.AppName);
    this.displayName = authService.getParsedJwtToken()?.name;
  }

  ngOnInit() {
  }

  logout() {
    this.authService.logout();
  }
}
