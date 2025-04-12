import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { NeoContextService } from './shared/services/neo-context.service';
import { LoaderGifService } from './shared/services/loader-gif.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
  <div *ngIf="loaderGifSvc.loading$ | async" class="overlay"></div>
  <div *ngIf="loaderGifSvc.loading$ | async" class="loading-img"></div>
  <router-outlet></router-outlet>
  `,
})
export class RootComponent {
  constructor(private titleSvc: Title, private ctx: NeoContextService, public loaderGifSvc: LoaderGifService) {
    this.titleSvc.setTitle(ctx.context.AppSettings!.AppName);
  }
}