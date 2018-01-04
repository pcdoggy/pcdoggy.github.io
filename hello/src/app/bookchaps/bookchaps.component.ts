import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { BookchapsService } from '../bookchaps.service';

@Component({
  selector: 'app-bookchaps',
  templateUrl: './bookchaps.component.html',
  styleUrls: ['./bookchaps.component.css']
})
export class BookchapsComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private bookchapsService: BookchapsService,
    private location: Location) { }

  ngOnInit() {
  }

  getHero(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.bookchapsService.getHero(id)
      .subscribe(hero => this.hero = hero);
  }

  goBack(): void {
    this.location.back();
  }
}
