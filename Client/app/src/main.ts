import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { RootComponent } from './root.component';
import { routes } from './shared/app.routes';
import { AuthInterceptor } from './shared/auth.interceptor';

bootstrapApplication(RootComponent, {
  providers: [provideRouter(routes), provideHttpClient(withInterceptors([AuthInterceptor]))]
}).catch(err => console.error(err));

