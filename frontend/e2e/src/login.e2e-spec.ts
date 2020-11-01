import { browser } from 'protractor';
import { LoginPage } from './login.po';

describe('Login tests', () => {
    let page: LoginPage;

    beforeEach(() => {
        page = new LoginPage();
        page.navigateTo();        
    });

    it('Login form should be valid', () => {
        if (!page.getEmailTextbox().getText())
            page.getEmailTextbox().sendKeys('main@mailinator.com');
            
        if (!page.getPasswordTextbox().getText())
            page.getPasswordTextbox().sendKeys('Main1718');

        
        let form = page.getForm().getAttribute('class');
        expect(form).toContain('ng-valid');
    });
});

