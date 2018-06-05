import { TemplateSyntaxPage } from './app.po';

describe('template-syntax App', () => {
  let page: TemplateSyntaxPage;

  beforeEach(() => {
    page = new TemplateSyntaxPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
