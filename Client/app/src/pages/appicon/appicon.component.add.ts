import {
  Component,
  ElementRef,
  EventEmitter,
  ViewChild,
  Input,
  inject,
  Output,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Observable, of, finalize, BehaviorSubject } from 'rxjs';
import { AppIcon, emptyAppIcon } from '../page-entities/appicon.entity';
import { DdlItem } from '../page-entities/ddl-item.entity';
import { AppIconService } from './appicon.service';
import {
  jQ,
  hideShowModal,
  validateForm,
  removeValidationErrors,
} from './../../shared/jquery-utils';

@Component({
  imports: [CommonModule, FormsModule],
  selector: 'add-modal',
  template: `
    <div
      #modal
      class="modal fade"
      data-backdrop="static"
      data-keyboard="false"
      tabindex="-1"
      role="dialog"
      aria-hidden="true"
      style="display: none;"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header box-header well">
            <h2 class="modal-title" id="model-title-h2">
              <i class="fa fa-plus"></i> Add New App Icon
            </h2>
            <div class="box-icon">
              <a
                href="#"
                data-dismiss="modal"
                data-neo-modal="true"
                class="btn btn-close btn-round btn-default"
              >
                <i class="glyphicon glyphicon-remove"></i>
              </a>
            </div>
          </div>
          <form
            #neoAddForm="ngForm"
            (submit)="onSubmit(neoAddForm)"
            class="box neo-add-form"
            novalidate
          >
            <div class="modal-body">
              <div class="row edit-form-field-container">
                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >Name
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="Name"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.Name"
                      type="text"
                      name="Name"
                      placeholder="Name"
                      class="form-control"
                      data-val="true"
                      data-val-required="The Name field is required."
                      autocomplete="off"
                    />
                  </div>
                </div>

                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >CssClass
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="CssClass"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.CssClass"
                      type="text"
                      name="CssClass"
                      placeholder="CssClass"
                      class="form-control"
                      data-val="true"
                      data-val-required="The CssClass field is required."
                      autocomplete="off"
                    />
                  </div>
                </div>

                <div class="col-lg-12">
                  <div class="form-group">
                    <label
                      >IconColor
                      <span
                        class="field-validation-valid"
                        data-valmsg-for="IconColor"
                        data-valmsg-replace="true"
                      ></span>
                    </label>
                    <input
                      [(ngModel)]="obj.IconColor"
                      type="text"
                      name="IconColor"
                      placeholder="IconColor"
                      class="form-control"
                      data-val="true"
                      data-val-required="The IconColor field is required."
                      autocomplete="off"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div class="modal-footer">
              <button
                type="button"
                class="btn btn-default btn-flat fa fa-times"
                data-dismiss="modal"
              >
                &nbsp;&nbsp;&nbsp;Close
              </button>
              <button type="submit" class="btn btn-info btn-flat fa fa-save">
                &nbsp;&nbsp;&nbsp;Save changes
              </button>
            </div>
            <div *ngIf="api.loading$ | async" class="overlay"></div>
            <div *ngIf="api.loading$ | async" class="loading-img"></div>
          </form>
        </div>
      </div>
    </div>
  `,
})
export class AddComponent {
  @ViewChild('neoAddForm', { read: ElementRef }) formElement!: ElementRef;
  @ViewChild('modal', { static: false }) modal!: ElementRef;
  @Output() shouldRefresh = new EventEmitter<boolean>();
  @Input() obj: AppIcon = emptyAppIcon();

  api = inject(AppIconService);
  companyList: DdlItem[] = [];

  private getModal(): HTMLElement {
    return this.modal.nativeElement;
  }

  loadForm(): void {
    removeValidationErrors(this.formElement);
    this.obj = emptyAppIcon();
    hideShowModal(this.getModal(), 'show');
  }

  onSubmit(form: NgForm): void {
    var isValid = validateForm(this.formElement);
    if (isValid) {
      this.obj.AppIconId = 0;
      this.obj.DemoTabs = null;
      this.obj.MenuCategories = null;
      this.obj.Webpages = null;
      this.api.add(this.obj).subscribe((resp) => {
        alert(resp.message);
        hideShowModal(this.getModal(), 'hide');
        this.shouldRefresh.emit(true);
      });
    }
  }
}
