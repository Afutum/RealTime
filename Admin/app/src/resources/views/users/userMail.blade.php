@extends('layouts.app')
@section('title','アカウント一覧')
@section('body')
    <h1>■ユーザー受信メール一覧</h1>
    <table class="table table-hover" id="table">
        <tr>
            <td>ID</td>
            <td>メールID</td>
            <th>ユーザー名</th>
            <th>受け取ったかどうか</th>
            <th>生成日</th>
            <th>最終更新日</th>
        </tr>
        @foreach($userMails as $userMail)
            <tr>
                <td>{{$userMail['id']}}</td>
                <td>{{$userMail['mail_id']}}</td>
                <td>{{$userMail['user_name']}}</td>
                <td>{{$userMail['is_received']}}</td>
                <td>{{$userMail['created_at']}}</td>
                <td>{{$userMail['updated_at']}}</td>
            </tr>
        @endforeach
        @if(!empty($error))
            <ul>
                <li class="text-danger">{{$error}}</li>
            </ul>
        @endif
    </table>
@endsection
