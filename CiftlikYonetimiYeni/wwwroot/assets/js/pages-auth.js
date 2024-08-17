/**
 *  Pages Authentication
 */

'use strict';
const formAuthentication = document.querySelector('#formAuthentication');

document.addEventListener('DOMContentLoaded', function (e) {
  (function () {
    // Form validation for Add new record
    if (formAuthentication) {
      const fv = FormValidation.formValidation(formAuthentication, {
        fields: {
          username: {
            validators: {
              notEmpty: {
                message: 'Lütfen Kullanıcı Adını Giriniz'
              },
              stringLength: {
                min: 5,
                message: 'Kullanıcı Adı en az 5 Karakter olmalıdır.'
              }
            }
          },
          email: {
            validators: {
              notEmpty: {
                message: 'Lütfen E-mail Adresininizi Giriniz'
              },
              emailAddress: {
                message: 'Lütfen Geçerli bir mail adresi girin'
              }
            }
          },
          'email-username': {
            validators: {
              notEmpty: {
                message: 'Lütfen Email ya da Kullanıcıcı Adı Giriniz'
              },
              stringLength: {
                min: 5,
                message: 'Kullanıcı Adı 5 Karakterden Fazla Olmalıdır'
              }
            }
          },
          password: {
            validators: {
              notEmpty: {
                message: 'Lütfen Parolanızı Giriniz'
              },
              stringLength: {
                min: 5,
                message: 'Parolanız En Az 5 Karakter Olmalıdır.'
              }
            }
          },
          'confirm-password': {
            validators: {
              notEmpty: {
                message: 'Lütfen Parolanızı Onaylayın'
              },
              identical: {
                compare: function () {
                  return formAuthentication.querySelector('[name="password"]').value;
                },
                message: 'Parolalar Uyuşmuyor'
              },
              stringLength: {
                min: 5,
                message: 'Parolanız en az 5 karakter olmalıdır'
              }
            }
          },
          terms: {
            validators: {
              notEmpty: {
                message: 'Lütfen Kullanıcı Sözleşmesini onaylayın'
              }
            }
          }
        },
        plugins: {
          trigger: new FormValidation.plugins.Trigger(),
          bootstrap5: new FormValidation.plugins.Bootstrap5({
            eleValidClass: '',
            rowSelector: '.mb-3'
          }),
          submitButton: new FormValidation.plugins.SubmitButton(),

          defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
          autoFocus: new FormValidation.plugins.AutoFocus()
        },
        init: instance => {
          instance.on('plugins.message.placed', function (e) {
            if (e.element.parentElement.classList.contains('input-group')) {
              e.element.parentElement.insertAdjacentElement('afterend', e.messageElement);
            }
          });
        }
      });
    }

    //  Two Steps Verification
    const numeralMask = document.querySelectorAll('.numeral-mask');

    // Verification masking
    if (numeralMask.length) {
      numeralMask.forEach(e => {
        new Cleave(e, {
          numeral: true
        });
      });
    }
  })();
});
