import { Input } from '@angular/core';
import { browser, by, element } from 'protractor';
export class LoginPage {
    navigateTo(){
        return browser.get('/auth/login');
    }

    getEmailTextbox() {
        return element(by.id('emailInput'));
    }

    getPasswordTextbox() {
        return element(by.id('passwordInput'));
    }

    getLoginButton() {
        return element(by.id('loginButton'));
    }

    getForm() {
        return element(by.id('loginForm'));
    }
}