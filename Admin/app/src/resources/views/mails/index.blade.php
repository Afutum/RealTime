@extends('layouts.app')
@section('title','アカウント一覧')
@section('body')
    <h1>■メール一覧</h1>
    <table class="table table-hover" id="table">
        <tr>
            <td>ID</td>
            <td>テキスト</td>
            <td>アイテム</td>
            <td>生成日</td>
            <td>最終更新日</td>
        </tr>
        @foreach($mails as $mail)
            <tr>
                <td>{{$mail['id']}}</td>
                <td>{{$mail['text']}}</td>
                <td>{{$mail['item_name']}}</td>
                <td>{{$mail['created_at']}}</td>
                <td>{{$mail['updated_at']}}</td>
            </tr>
        @endforeach
        @if(!empty($error))
            <ul>
                <li class="text-danger">{{$error}}</li>
            </ul>
        @endif
    </table>
@endsection
