
# simple script to create markdown list of all articles on ericlippert.com

use utf8::all;
use Mojo::UserAgent;
use Data::Dump;

my $ua = Mojo::UserAgent->new(max_redirects => 5);

my $base_url = 'https://ericlippert.com';

# dd $ua->get($base_url)->res;

for my $month ($ua->get($base_url)->res->dom->find('#archives-2 a')->each) {
    warn $month->text, "\n";
    print "\n## [", $month->text, "](", $month->{href}, ")\n";
    for my $article ($ua->get($month->{href})->res->dom->find('article')->each) {
        my $title = $article->find('.entry-title > a:nth-child(1)')->first;
        my @tags  = $article->find('span.tag-links > a')->each;
        # join(', ', map { $_->text } @tags) can be used to obtain texts, but does not seem useful
        print " - [ ] [", $title->text, "](", $title->{href}, ")\n";
    }
}

