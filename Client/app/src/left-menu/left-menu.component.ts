import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { initLeftMenu } from './../shared/jquery-utils';
import { AuthService } from './../shared/services/auth.service';
import { Menu, emptyMenu } from '../shared/menu.entity';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html',
  styleUrls: ['./left-menu.component.css'],
  imports: [CommonModule, RouterModule]
})
export class LeftMenuComponent implements OnInit {

  constructor(private svc: AuthService) { }

  menu: Menu = emptyMenu();

  ngOnInit() {
    this.svc.getLeftMenu().subscribe(data => {
      this.menu = data;
      setTimeout(() => {
        initLeftMenu();
      }, 0);
    });
  }
}
