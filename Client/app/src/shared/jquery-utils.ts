import { ElementRef } from '@angular/core';

declare var $: any;

export const initLeftMenu = () => {
    $('.accordion-toggle-a,.accordion-toggle-a > i, .accordion-toggle-a > sapn').click(function (event: any) {
        console.log(event.target);
        var $ul = $(event.target).siblings('ul');
        if ($(event.target).prop('tagName').toLowerCase() != 'a') {
            event.preventDefault();
            event.stopPropagation();
            $ul = $(event.target).parent().siblings('ul');
        }
        // $('.accordion-toggle-a').siblings('ul').slideUp();
        $ul.slideToggle();
    });
    $('.accordion li.active:first').parents('ul').slideDown();
};

export const initPagination = (elem: string, currentPageNo: number, totalPageCount: number, onPageClick: Function) => {
    const selector = `div[data-pagination-id='${elem}']`;
    $(selector).empty();
    $(selector).bootpag({
        page: currentPageNo,
        total: totalPageCount,
        maxVisible: 5,
        firstLastUse: true,
        first: 'FIRST',
        last: 'LAST'
    }).off('page').on('page', function (event: any, num: number) {
        onPageClick(num - 1);
    });
};

export const hideShowModal = (elm: HTMLElement, action: string) => {
    $(elm as any).modal(action);
};

export const jQ = (elm: String): any => {
    return $(elm as any);
};

export const validateForm = (formSelector: ElementRef) => {
    var form = $(formSelector.nativeElement);
    form.data('validator', null);
    $.validator.unobtrusive.parse(form);
    form.validate();
    return form.valid();
};

export const removeValidationErrors = (formSelector: ElementRef) => {
    var form = $(formSelector.nativeElement);
    var validator = form.validate({
        errorPlacement: function (error: any, element: any) {
        }
    });
    $(form).removeData('validator');
    $(form).removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
    validator.resetForm();
    $(formSelector.nativeElement).find('input,select').removeClass('input-validation-error');
};

export const select2 = (elm: HTMLElement, modal: any = null) => {
    let config = {
        templateResult: function (state: any) {
            if (!state.id) return state.text;
            return $(state.element).data('html') || state.text;
        },
        templateSelection: function (state: any) {
            console.log(state);
            return $(state.element).data('html') || state.text;
        },
        escapeMarkup: function (markup: any) {
            return markup;
        },
        dropdownParent: null
    };

    if (modal != null) {
        config.dropdownParent = $(modal);
    }

    $(elm).select2(config);

    // Trigger 'change' event when Select2 value is updated
    $(elm).on('change', function () {
        const selectedValue = $(elm).val();
        $(elm).triggerHandler('ngModelChange', selectedValue);
    });
};

export const initLoginSlick = () => {
    setTimeout(function () {
        $('.slick-slider').slick({
            dots: true,
            slidesToShow: 1,
            slidesToScroll: 1,
        });

        $('.slick-slider-3').slick({
            dots: true,
            slidesToShow: 1,
            slidesToScroll: 1,
        });

        $('.slick-slider-2').slick({
            className: 'center',
            centerMode: true,
            infinite: true,
            centerPadding: '60px',
            slidesToShow: 3,
            speed: 500,
            dots: true,
        });

        $('.slick-slider-variable').slick({
            className: 'slider variable-width',
            dots: true,
            infinite: true,
            centerMode: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            variableWidth: true,
        });

        $('.slick-slider-responsive').slick({
            dots: true,
            infinite: false,
            speed: 500,
            slidesToShow: 4,
            slidesToScroll: 4,
            initialSlide: 0,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3,
                        infinite: true,
                        dots: true,
                    },
                },
                {
                    breakpoint: 600,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        initialSlide: 2,
                    },
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
            ],
        });

        $('.slick-slider-inverted').slick({
            infinite: true,
            slidesToShow: 1,
            speed: 500,
            dots: true,
            adaptiveHeight: true,
        });
        setTimeout(() => {
            $('#login-div').show();
        }, 0);
    }, 0);
};