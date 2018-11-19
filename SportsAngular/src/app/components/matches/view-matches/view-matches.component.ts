import { 
  Component, 
  NgModule,
  ViewChild,
  TemplateRef } from '@angular/core';
  import { Router, ActivatedRoute } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { MatchService } from '../../../services/match.service';
import { AlertService } from '../../../services/alert.service';
import { SportsService } from '../../../services/sports.service';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { Match } from "../../../classes/match";
import { Competitor } from "../../../classes/competitor";
import { Sport } from "../../../classes/sport";
import {
  startOfDay,
  endOfDay,
  subDays,
  addDays,
  endOfMonth,
  isSameDay,
  isSameMonth,
  addHours
} from 'date-fns';
import { Subject } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
  CalendarEvent,
  CalendarEventAction,
  CalendarEventTimesChangedEvent,
  CalendarView,
  CalendarMonthViewDay 
} from 'angular-calendar';
import { CalendarEventActionsComponent } from 'angular-calendar/modules/common/calendar-event-actions.component';
const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3'
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF'
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA'
  }
};

@Component({
  selector: 'app-view-matches',
  templateUrl: './view-matches.component.html',
  styleUrls: ['./view-matches.component.css']
})
export class ViewMatchesComponent  {

  @ViewChild('modalContent')
  modalContent: TemplateRef<any>;

  sports: Array<Sport>;
  allSports: Array<Sport>;
  comptetitors: Array<Competitor>;
  selectedSport:Sport;
  sportId:number;
  selectedCompetitor:Competitor;
  view: CalendarView = CalendarView.Month;

  CalendarView = CalendarView;

  viewDate: Date = new Date();

  modalData: {
    action: string;
    event: CalendarEvent;
  };

  actions: CalendarEventAction[] = [
    {
      label: '<i class="fa fa-fw fa-pencil"></i>',
      onClick: ({ event }: { event: CalendarEvent }): void => {
        this.router.navigate(['/matches/modify/, event.id']);
      }
    }
  ];

  
  refresh: Subject<any> = new Subject();

  events: CalendarEvent[] = [];
  activeDayIsOpen: boolean = true;

  constructor(
    private matchService: MatchService,
    private alertService: AlertService,
    private sportService: SportsService,
    private modal: NgbModal,
    private router: Router
    ) { }

  ngOnInit(): void {
    this.getAllMatches();
    
  }

  getAllMatches(){
    this.activeDayIsOpen = false;
    this.sports = new Array<Sport>();
    this.comptetitors = new Array<Competitor>();
    this.matchService.getAllMatches().subscribe(
      ((obtainedSports) => {
          this.sports = obtainedSports;
          this.allSports = obtainedSports;
          this.fillWithSports();
          this.refresh.next();
      }),
      ((error: any) => {
          this.alertService.error(error.message);
      })
  );
  }
  fillWithSports(){
    this.events = [];
    this.sports.forEach(sport => {
      sport.matches.forEach(match => {
        let datetime = match.date.split(" ");
        let splitDate = datetime[0].split("/");
        let stringDate = splitDate[1]+"/"+splitDate[0]+"/"+splitDate[2];
        let date = new Date(stringDate);
        let competitorsString = "";
        match.competitors.forEach(competitor => {
          competitorsString+=competitor.competitor.name+" ("+competitor.score+") - ";
        });
        competitorsString+=datetime[1];
        this.events.push( {
          title: competitorsString,
          color: colors.yellow,
          id : match.id,
          start: date,
          meta: {
            type: sport.name
          }
        });
      });
      
    });
  }

  beforeMonthViewRender({ body }: { body: CalendarMonthViewDay[] }): void {
    body.forEach(cell => {
      const groups: any = {};
      cell.events.forEach((event: CalendarEvent<{ type: string }>) => {
        groups[event.meta.type] = groups[event.meta.type] || [];
        groups[event.meta.type].push(event);
      });
      cell['eventGroups'] = Object.entries(groups);
    });
  }

  dayClicked({ date, events }: { date: Date; events: CalendarEvent[] }): void {
    if (isSameMonth(date, this.viewDate)) {
      this.viewDate = date;
      if (
        (isSameDay(this.viewDate, date) && this.activeDayIsOpen === true) ||
        events.length === 0
      ) {
        this.activeDayIsOpen = false;
      } else {
        this.activeDayIsOpen = true;
      }
    }
  }

  onSportSelected(value){
    this.activeDayIsOpen = false;
    if(value!=0){
    this.matchService.getAllMatchesBySport(value).subscribe(
      ((obtainedSports) => {
          this.sports = obtainedSports;
          this.fillWithSports();
          this.refresh.next();
      }),
      ((error: any) => {
          this.alertService.error(error.message);
      })
  );
   this.sportService.getSportById(value).subscribe(
      data => {
        this.selectedSport = data;
        this.comptetitors = this.selectedSport.competitors;
      },
      error => {
          this.alertService.error(error.message);
      }
    );
  }else{
    this.getAllMatches();
  }
  }

  onCompetitorSelected(value){
    this.activeDayIsOpen = false;
    if(value!=0){
      this.matchService.getAllMatchesByCompetitor(value).subscribe(
        ((obtainedSports) => {
            this.sports = obtainedSports;
            this.fillWithSports();
            this.refresh.next();
        }),
        ((error: any) => {
            this.alertService.error(error.message);
        })
    );
    }else{
      this.onSportSelected(this.sportId);
    }
    
  }

  
  handleEvent(action: string, event: CalendarEvent): void {
    this.modalData = { event, action };
    let route = 'matches/'+event.id;
    this.router.navigate([route]);
  }

}
