@extends('layouts.app')
@section('title','アカウント登録')
@section('body')
    @section('userList','nav-link px-2 ')
    @section('userLeg','nav-link px-2 link-secondary')
    @section('player','nav-link px-2')
    @section('item','nav-link px-2')
    @section('personalItem','nav-link px-2')
    <form method="post" action="{{route('accounts.store')}}">
        @csrf
        <div class="modal modal-sheet position-static d-block bg-body-secondary p-4 py-md-5" tabindex="-1" role="dialog"
             id="modalSignin">
            <div class="modal-dialog" role="document">
                <div class="modal-content rounded-4 shadow">
                    <div class="modal-header p-5 pb-4 border-bottom-0">
                        <h1 class="fw-bold mb-0 fs-2">ユーザー登録</h1>
                    </div>
                    <div class="modal-body p-5 pt-0">
                        <form class="">
                            <div class="form-floating mb-3">
                                <input type="text" class="form-control rounded-3" id="floatingInput" name="name">
                                <label for="floatingInput">ユーザー名</label>
                            </div>
                            <div class="form-floating mb-3">
                                <input type="password" class="form-control rounded-3" id="floatingPassword"
                                       placeholder="Password" name="password">
                                <label for="floatingPassword">パスワード</label>
                            </div>
                            <div class="form-floating mb-3">
                                <input type="password" class="form-control rounded-3" id="floatingPassword"
                                       placeholder="Password" name="password_confirmation">
                                <label for="floatingPassword">パスワード(再確認)</label>
                            </div>
                            <button class="w-100 mb-2 btn btn-lg rounded-3 btn-primary" type="submit">登録</button>
                            <input type="hidden" name="action" value="doLogin">
                        </form>
                        @if(!empty($success))
                            <ul>
                                <li>{{$success}}</li>
                            </ul>
                        @endif
                        @if(!empty($error))
                            <ul>
                                <li class="text-danger">その名前はすでに使われています</li>
                            </ul>
                        @endif
                        @if(!empty($errors))
                                <?php \Barryvdh\Debugbar\Facades\Debugbar::info($errors); ?>
                            @if($errors->any())
                                <ul>
                                    @foreach($errors->all() as $error)
                                        <li class="text-danger">{{$error}}</li>
                                    @endforeach
                                </ul>
                            @endif
                        @endif
                    </div>
                </div>
            </div>
        </div>
    </form>
@endsection
