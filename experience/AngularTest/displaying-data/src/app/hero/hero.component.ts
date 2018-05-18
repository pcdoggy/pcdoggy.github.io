import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-hero',
  template: `
    <p>
      hero Works!
    </p>
  `,
  styleUrls: ['./hero.component.css']
})
export class HeroComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
