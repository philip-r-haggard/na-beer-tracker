import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_modules/member';
import { Observable, map, of } from 'rxjs';
import { Entry } from '../_modules/entry';
import { EntryDto } from '../_models/entryDto';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  usersWithEntries: any[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(members => {
        this.members = members;
        return members;
      })
    )
  }

  getMember(username: string) {
    const member = this.members.find(x => x.userName === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username)
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {...this.members[index], ...member}
      })
    )
  }

  getEntriesForUser(username: string): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'users/' + username + '/entries');
  }

  getMemberWithEntries(): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl + 'users/withentries');
  }

  // Method to create a new entry for a user
  createEntry(entryDto: EntryDto): Observable<any> {
    return this.http.post<any>(this.baseUrl + 'entries/create', entryDto);
  }
  

  // Method to update an existing entry
  updateEntry(entryDto: EntryDto, entryId: number): Observable<any> {
    return this.http.put(this.baseUrl + 'entries/update/' + entryId, entryDto);
  }

  // Method to delete an entry
  deleteEntry(entryId: number) {
    return this.http.delete(this.baseUrl + 'entries/delete/' + entryId);
  }
}
