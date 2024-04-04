import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { EntryDto } from 'src/app/_models/entryDto';
import { User } from 'src/app/_models/user';
import { Member } from 'src/app/_modules/member';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

// Define the interface for Entry
interface Entry {
  id?: number; // Make id property optional as it might not exist initially
  title?: string;
  description?: string;
}

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;
  userEntries: Entry[] = [];
  newEntryTitle: string = '';
  newEntryDescription: string = '';
  editedEntry: Entry = {};

  constructor(private accountService: AccountService, private memberService: MembersService,
    private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    });
  }

  ngOnInit(): void {
    this.loadMember();
    this.loadUserEntries(this.user!.username);
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    });
  }

  loadUserEntries(username: string): void {
    this.memberService.getEntriesForUser(username).subscribe(data => {
      this.userEntries = data;
    });
  }

  updateMember() {
    this.memberService.updateMember(this.member!).subscribe({
      next: _ => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    });
  }

  createEntry(username: string): void {
    const entryDto: EntryDto = {
      title: this.newEntryTitle,
      description: this.newEntryDescription,
      username: this.member!.userName
    };
    this.memberService.createEntry(entryDto).subscribe(() => {
      this.loadUserEntries(this.user!.username); // Reload the user entries after creating a new entry
      this.clearEntryForm(); // Clear the entry form after creating a new entry
    });
  }

  editEntry(entry: Entry): void {
    this.editedEntry = { ...entry }; // Copy the entry object to edit
  }

  updateEntry(entry: Entry): void {
    const entryDto: EntryDto = {
      title: this.editedEntry.title!,
      description: this.editedEntry.description!,
      username: this.member?.userName!
    };

    // Assuming you have a service method to update an entry
    this.memberService.updateEntry(entryDto, entry.id ?? 0).subscribe(() => {
      this.toastr.success('Entry updated successfully');
      this.loadUserEntries(this.user!.username); // Reload the user entries after updating
      this.editedEntry = {}; // Clear the editedEntry object
    }, error => {
      this.toastr.error('Failed to update entry');
      console.error(error); // Log any errors to the console
    });
  }



  deleteEntry(entryId?: number): void {
    if (entryId !== undefined && entryId !== null) {
      this.memberService.deleteEntry(entryId).subscribe(() => {
        // Remove the deleted entry from the userEntries array
        this.userEntries = this.userEntries.filter(entry => entry.id !== entryId);
        // Optionally, you can also reload the member data if needed
        // this.loadMember();
      });
    } else {
      // Handle the case where entryId is undefined or null
      console.error('Cannot delete entry: Invalid entry ID');
    }
  }


  clearEntryForm(): void {
    this.newEntryTitle = '';
    this.newEntryDescription = '';
  }
}
