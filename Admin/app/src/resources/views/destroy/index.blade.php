@extends('layouts.app')
@section('title','アカウント削除確認')
<body>
<div class="modal modal-sheet position-static d-block bg-body-secondary p-4 py-md-5" tabindex="-1" role="dialog"
     id="modalSignin">
    <div class="modal-dialog" role="document">
        <div class="modal-content rounded-4 shadow">
            <div class="modal-header p-8 pb-5 border-bottom-0">
                <h1 class="fw-bold mb-0 fs-2">「{{$account['name']}}」を削除しますか？</h1>
                <form method="post" action="{{route('accounts.successDestroy')}}">
                    @csrf
                    <button type="submit">削除</button>
                    <input type="hidden" name="id" value={{$account['id']}}>
                </form>
                <form method="post" action="{{route('accounts.deleteCansel')}}">
                    @csrf
                    <button type="submit">キャンセル</button>
                    <input type="hidden" name="id" value={{$account['id']}}>
                </form>
            </div>
        </div>
    </div>
</div>
