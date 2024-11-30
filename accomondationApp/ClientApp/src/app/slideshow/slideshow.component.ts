import { Component, Input, OnInit, NgModule, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Slide } from '../interfaces/Slide';
import { PicturePath } from '../interfaces/PicturePath';
import { SlideService } from '../SlideService';

@Component({
  selector: 'app-slideshow',
  templateUrl: './slideshow.component.html',
  styleUrls: ['./slideshow.component.css']
})
export class SlideshowComponent implements OnInit{
  slideService: SlideService = Inject(SlideService);
  @Input() slides: Slide[];
  @Input() selectedSlide: Slide;
  selectedSlideIndex : number = 0;
  currentSlides: number[] = [];
  currentSlideSelected: number[] = [];

  constructor(private _slideService: SlideService){
    this.slideService = _slideService;
    this.slideService.getSlides().subscribe(result =>
      this.slides = result,
      error => console.error(error),
      () => console.log('Slides loaded'),
    );
  }

  ngOnInit(): void {
    this.slideService.getSlides().subscribe(result =>
      this.slides = result,
      error => console.error(error),
      () => {
        console.log('Slides loaded');
        for(let i = 0; i < this.slides.length; i++){
          this.currentSlides.push(0);
          this.currentSlideSelected.push(0);
        }
        this.selectedSlide = this.slides[0];
      },
    );
  }

  onPreviousClick(){
    const previous = this.currentSlides[this.selectedSlideIndex] - 1;
    this.currentSlides[this.selectedSlideIndex] = previous < 0 ? this.slides[this.selectedSlideIndex].picturePaths.length - 1 : previous;
    console.log("previous clicked, new current slide is: ", this.currentSlides[this.selectedSlideIndex]);
  }

  onNextClick(){
    const next = this.currentSlides[this.selectedSlideIndex] + 1;
    this.currentSlides[this.selectedSlideIndex] = next === this.slides[this.selectedSlideIndex].picturePaths.length ? 0 : next;
    console.log("next clicked, new current slide is: ", this.currentSlides[this.selectedSlideIndex]);
  }
  clearCurrentSlideSelected(){
      
  }

  selectRoom(row: number){
      this.selectedSlide = this.slides[row];
      this.selectedSlideIndex = row;
  }

}
